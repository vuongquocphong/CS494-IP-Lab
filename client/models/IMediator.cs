using GameComponents;
using Messages;

namespace Mediator
{
    public enum Event
    {
        // Input name scene
        REQUEST_CONNECT, // when player presses PLAY

        CONNECT_SUCCESS, // when there is no error

        CONNECT_FAILURE, // when there are errors


        // Waiting scene
        REQUEST_PLAYER_WAITING_LIST, // when player connects successfully

        UPDATE_PLAYER_WAITING_LIST, // when player presses READY

        SET_PLAYER_WAITING_LIST, // when server receives updates from other clients

        DISCONNECT, // when player presses BACK

        START_GAME, // when all players are ready


        // Playing scene
        NEW_WORD, // when the game first starts

        GUESS_CHAR, // when player guess char and presses SUBMIT

        GUESS_KEYWORD, // when player guess keyword and presses SUBMIT

        UPDATE_GAME_STATUS, // when a new update occurs to the current game status

        WRONG_GUESS_CHARACTER, // when a char guess is wrong

        WRONG_GUESS_KEYWORD, // when a keyword guess is wrong

        TIMEOUT, // when the guess exceed the allowed time

                //DISCONNECT has already been defined above when player presses EXIT

                
        // Scoreboard scene
        REQUEST_SCOREBOARD, // when player finishes the game

        RESTART, //when player presses RESTART

                // DISCONNECT has already been defined above when player presses QUIT
    }
    public interface IMediator
    {
        abstract void Notify(object sender, Event ev);
    }
}