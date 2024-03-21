import socket
import threading
import random
import time
import signal
import sys

class Player:
    def __init__(self, conn, addr, nickname):
        self.conn = conn
        self.addr = addr
        self.nickname = nickname
        self.points = 0

class GameServer:
    def __init__(self, host, port, max_players):
        self.host = host
        self.port = port
        self.max_players = max_players
        self.players = []
        self.current_turn = 0
        self.keyword = ""
        self.description = ""
        self.keyword_length = 0
        self.guesses = []
        self.turn_count = 0
        self.game_over = False
        self.server_socket = None

    def run(self):
        self.server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.server_socket.bind((self.host, self.port))
        self.server_socket.listen(self.max_players)
        print("Server started, waiting for connections...")

        # Register a signal handler for graceful shutdown
        signal.signal(signal.SIGINT, self.shutdown_handler)

        while True:
            conn, addr = self.server_socket.accept()
            print(f"New connection from {addr}")
            player_thread = threading.Thread(target=self.handle_player, args=(conn, addr))
            player_thread.start()

    def shutdown_handler(self, signum, frame):
        print("Shutting down server...")
        self.server_socket.close()
        sys.exit(0)

    def handle_player(self, conn, addr):
        conn.sendall(b"Welcome to The Magical Wheel game!\n")
        conn.sendall(b"Please choose a nickname: ")
        nickname = conn.recv(1024).decode().strip()

        while self.nickname_exists(nickname):
            conn.sendall(b"This nickname is already taken. Please choose another one: ")
            nickname = conn.recv(1024).decode().strip()

        player = Player(conn, addr, nickname)
        self.players.append(player)
        print(f"Player {nickname} registered successfully.")

        if len(self.players) == self.max_players:
            self.start_game()

    def nickname_exists(self, nickname):
        for player in self.players:
            if player.nickname == nickname:
                return True
        return False

    def start_game(self):
        self.load_keyword()
        self.broadcast(f"Registration completed successfully. The order of players is: {', '.join([player.nickname for player in self.players])}\n")
        self.play_game()

    def load_keyword(self):
        with open("database.txt", "r") as file:
            lines = file.readlines()
            n = int(lines[0])
            index = random.randint(1, n)
            keyword_line = lines[index * 2 - 1].strip().lower()
            description = lines[index * 2].strip()
            self.keyword = keyword_line.split()[0]
            self.description = description
            self.keyword_length = len(self.keyword)
            self.guesses = ['*'] * self.keyword_length
            self.turn_count = 0

    def broadcast(self, message):
        for player in self.players:
            player.conn.sendall(message.encode())

    def play_game(self):
        while not self.game_over:
            current_player = self.players[self.current_turn]
            self.broadcast(f"It's {current_player.nickname}'s turn. The length of the keyword is {self.keyword_length}: {' '.join(self.guesses)}\n")
            self.broadcast(f"Hints: {self.description}\n")

            current_player.conn.sendall(b"Your turn. Guess a character: ")
            guess = current_player.conn.recv(1024).decode().strip().lower()

            if len(guess) == 1:
                if guess in self.keyword:
                    count = self.keyword.count(guess)
                    self.update_guesses(guess)
                    self.broadcast(f"Character '{guess}' has {count} occurrence(s).\n")
                    self.broadcast(f"The current keyword is {' '.join(self.guesses)}\n")
                    if '*' not in self.guesses:
                        self.game_over = True
                        self.broadcast(f"Congratulations to the winner with the correct keyword: {self.keyword}\n")
                else:
                    self.broadcast(f"Character '{guess}' is not in the keyword.\n")
                    self.next_turn()
            else:
                self.broadcast("Invalid guess. Please guess a single character.\n")

            self.current_turn = (self.current_turn + 1) % len(self.players)
            self.turn_count += 1

            if self.turn_count >= 5 and not self.game_over:
                self.game_over = True
                self.broadcast("No one guessed the keyword. Game over.\n")

        self.end_game()

    def update_guesses(self, guess):
        for i, char in enumerate(self.keyword):
            if char == guess:
                self.guesses[i] = char

    def next_turn(self):
        self.current_turn = (self.current_turn + 1) % len(self.players)

    def end_game(self):
        self.calculate_points()
        self.broadcast("Game over. Final scores:\n")
        for player in self.players:
            self.broadcast(f"{player.nickname}: {player.points} points\n")
        self.players = []
        self.game_over = False

    def calculate_points(self):
        for player in self.players:
            if ''.join(self.guesses) == self.keyword:
                player.points += 5
            else:
                for guess in self.guesses:
                    if guess in self.keyword:
                        player.points += 1

if __name__ == "__main__":
    server = GameServer('localhost', 5555, 2)  # Change max_players as desired
    server.run()
    
