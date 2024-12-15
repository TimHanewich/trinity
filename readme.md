# chess3
My third attempt at a chess engine.

## FEN Notations
- Start of game: `rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1`
- Challenging mid-games:
    - `r5k1/1p3p2/2p2qpb/2PpR3/3P2r1/pP1Q1NP1/P4PK1/7R b - - 1 33`
    - `3r2k1/1ppq1pb1/p1n3pp/3N1n2/2PP2P1/1P1Q1N2/PB3P1P/4R1K1 b - - 0 17`
    - `4rk2/pp1b1pp1/8/4P3/2p1R3/2Pp1PB1/PP5P/6K1 b - - 1 27`
    - `r2q2k1/1p3pb1/2p3p1/2PpR1N1/1P1Q4/p5P1/P4PK1/7R b - - 2 36` - this one has a multi-move take-take-take combo that it should pick up on
- Interesting end-games:
    - `8/6k1/2R5/2p3p1/8/6PP/5PK1/2r5 b - - 4 45`
- End-games where there is a massive advantage and the next move should be obvious:
    - `7k/6p1/8/1b6/8/1P6/4Q3/K7 b - - 0 1`

## Performance Tests
Each test below finds the best optimal next state at a search depth of 5 for the position `r5k1/1p3p2/2p2qpb/2PpR3/3P2r1/pP1Q1NP1/P4PK1/7R b - - 1 33`.
|Commit|Time (seconds)|Note|
|-|-|-|
|[chess2](https://github.com/TimHanewich/chess2) @ commit `aa15331b3d77e629737d54b35c79ea01f5869352`|38.16||
|`cd9a704b4b1e231a1aea96c04488566c9e9d7f4b`|5.13|Uses bitboard, unlike `chess2`. Pre-optimization.|
|`f5f58f117b946e302001225743af0e94e3b6ee80`|5.40|Added pawn promotion consideration|
|`a888ac38ea77c73e0a9c05d80533117125ca68fc`|7.20|Added center-of-board evaluation as part of static positional eval|
|`eb0de8d4b55e7bc46ab37212f861c93b79c468e0`|8.5|Added pawn ranking evaluation as part of static positional eval|