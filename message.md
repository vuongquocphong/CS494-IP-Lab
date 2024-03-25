# General Message Format
|Message type|...|
|------------|---|
|1 byte      |   |

## Client
|Message type value | Meaning                   |
|-------------------|---------------------------|
|0                  |Client Connection Message  |
|1                  |Ready/Unready              |
|2                  |Guess                      |
|3                  |Timeout                    |

## Server
|Message type value | Meaning                    |
|-------------------|----------------------------|
|4                  |Server Connection Message   |
|5                  |PlayerList                  |
|6                  |Game Started                |
|7                  |Game Status                 |
|8                  |Guess Result                |
|9                  |Game Result                 |

# Client Messages
## Client Connection Message
|Message type|
|------------|
|1 byte      |

## Ready/Unready
|Message type|Ready |
|------------|------|
|1 byte      |1 byte|

|Ready value    |Meaning    |
|---------------|-----------|
|0              |Unready    |
|1              |Ready      |

## Guess
|Message type|Guess Type|Value  |
|------------|----------|-------|
|1 byte      |1 byte    | 4 byte|

|Guess Type |Meaning |
|-----------|--------|