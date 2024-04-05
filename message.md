# General Message Format

| Total Length | Message type | ... |
| ------------ | ------------ | --- |
| 2 bytes      | 1 byte       |     |

## Client

| Message type value | Meaning                   |
| ------------------ | ------------------------- |
| 0                  | Client Connection Message |
| 1                  | Ready/Unready             |
| 2                  | Guess                     |
| 3                  | Timeout                   |

## Server

| Message type value | Meaning                   |
| ------------------ | ------------------------- |
| 4                  | Server Connection Success |
| 5                  | Server Connection Failure |
| 6                  | Player List               |
| 7                  | Game Status               |
| 8                  | Guess Result              |
| 9                  | Game Result               |

# Client Messages

## Client Connection Message

| Message type | Name     |
| ------------ | -------- |
| 1 byte       | 10 bytes |

## Ready/Unready

| Message type | Ready  |
| ------------ | ------ |
| 1 byte       | 1 byte |

| Ready value | Meaning |
| ----------- | ------- |
| 0           | Unready |
| 1           | Ready   |

## Guess

| Message type | Guess Type | Guess    |
| ------------ | ---------- | -------- |
| 1 byte       | 1 byte     | 30 bytes |

| Guess Type | Meaning          |
| ---------- | ---------------- |
| 0          | single character |
| 1          | full keyword     |

## Timeout

| Message type |
| ------------ |
| 1 byte       |

# Server Messages

## Server Connection Success

| Message type |
| ------------ |
| 1 byte       |

## Server Connection Failure

| Message type | Error Code | Error Message |
| ------------ | ---------- | ------------- |
| 1 byte       | 1 byte     | max 128 bytes |

| Error code | Error message         |
| ---------- | --------------------- |
| 0          | Invalid name          |
| 1          | Game in progress      |
| 2          | Server is full        |
| 3          | Internal server error |
| 4          | Name already taken    |

## PlayerList

| Message type | Player Count | Name Length 1 | Player Name 1 | Player 1 Ready Status | Name Length 2 | Player Name 2 | Player 2 Ready Status | ... |
| ------------ | ------------ | ------------- | ------------- | --------------------- | ------------- | ------------- | --------------------- | --- |
| 1 byte       | 1 byte       | 1 byte        | 10 bytes      | 1 byte                | 1 byte        | 10 bytes      | 1 byte                | ... |

| Player Ready value | Meaning |
| ------------------ | ------- |
| 0                  | Unready |
| 1                  | Ready   |

## Game Started

| Message type | Keyword  |
| ------------ | -------- |
| 1 byte       | 30 bytes |

## Game Status

| Message type | Player Count | Game Turn | Current Turn | Keyword Length | Keyword  | Player Info 1 | Player Info 2 | ... |
| ------------ | ------------ | --------- | ------------ | -------------- | -------- | ------------- | ------------- | --- |
| 1 byte       | 1 byte       | 1 byte    | 1 byte       | 1 byte         | 30 bytes | max 15 bytes  | max 15 bytes  | ... |

### Player Info

| Name Length | Player Name | Player Score | Player State | Turn order |
| ----------- | ----------- | ------------ | ------------ | ---------- |
| 1 byte      | 10 bytes    | 2 bytes      | 1 bytes      | 1 bytes    |

#### Player State

| Value | Meaning      |
| ----- | ------------ |
| 0     | Playing      |
| 1     | Lost         |
| 2     | Disconnected |
| 3     | Win          |

#### Turn order

Ranges from 0-9.

## Guess Result

| Message type | Result | Guess Type | Guess    |
| ------------ | ------ | ---------- | -------- |
| 1 byte       | 1 byte | 1 byte     | 30 bytes |

| Result value | Meaning         |
| ------------ | --------------- |
| 0            | Correct         |
| 1            | Incorrect       |
| 2            | Duplicate guess |
| 3            | Invalid guess   |

## Game Result

| Message type | Player Count | Player Result 1 | Player Result 2 | ... |
| ------------ | ------------ | --------------- | --------------- | --- |
| 1 byte       | 1 byte       | max 14 bytes    | max 14 bytes    | ... |

### Player Result

| Name Length | Player Name  | Player Score | Player Rank |
| ----------- | ------------ | ------------ | ----------- |
| 1 byte      | max 10 bytes | 2 bytes      | 1 byte      |

#### Player Rank

Ranges from 1-10.
