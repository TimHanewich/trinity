# chess3
My third attempt at a chess engine.

## FEN Notations
- Start of game: `rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1`
- Challenging mid-games:
    - `r5k1/1p3p2/2p2qpb/2PpR3/3P2r1/pP1Q1NP1/P4PK1/7R b - - 1 33`
- Interesting end-games:
    - `8/6k1/2R5/2p3p1/8/6PP/5PK1/2r5 b - - 4 45`

## Performance Tests
Each test below finds the best optimal next state at a search depth of 5 for the position `r5k1/1p3p2/2p2qpb/2PpR3/3P2r1/pP1Q1NP1/P4PK1/7R b - - 1 33`.
|Commit|Time (seconds)|Note|
|-|-|-|
|[chess2](https://github.com/TimHanewich/chess2) @ commit `aa15331b3d77e629737d54b35c79ea01f5869352`|38.16||
|`cd9a704b4b1e231a1aea96c04488566c9e9d7f4b`|5.13|Uses bitboard, unlike `chess2`. Pre-optimization.|
|`f5f58f117b946e302001225743af0e94e3b6ee80`|5.40|Added pawn promotion consideration|
|`a888ac38ea77c73e0a9c05d80533117125ca68fc`|7.20|Added center-of-board evaluation as part of static positional eval|
|`eb0de8d4b55e7bc46ab37212f861c93b79c468e0`|8.5|Added pawn ranking evaluation as part of static positional eval|