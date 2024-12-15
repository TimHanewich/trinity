# chess3
My third attempt at a chess engine.

## FEN Notations
- Start of game: `rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1`
- Challenging mid-games:
    - `r5k1/1p3p2/2p2qpb/2PpR3/3P2r1/pP1Q1NP1/P4PK1/7R b - - 1 33`

## Performance Tests
Each test below finds the best optimal next state at a search depth of 5 for the position `r5k1/1p3p2/2p2qpb/2PpR3/3P2r1/pP1Q1NP1/P4PK1/7R b - - 1 33`.
|Commit|Time (seconds)|
|-|-|
|[chess2](https://github.com/TimHanewich/chess2) @ commit ``|38.16|
|`cd9a704b4b1e231a1aea96c04488566c9e9d7f4b`|5.13|