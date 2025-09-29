# Chess AI: Single-File C# Bot for Sebastian Lague’s Coding Challenges

This repository features a **compact Chess AI** implemented in a single C# file, **`MyBot.cs`**, originally designed for [Sebastian Lague’s Chess Challenge](https://github.com/SebLague/Chess-Challenge)

## Project Overview

- **Single-file, 500-token limit**: The bot is written in under 500 tokens (as noted in the code comments), adhering to challenge constraints.  
- **Implements `IEvaluator`**: Uses the `ChessChallenge.API` interface to evaluate board positions for each move.  
- **Bitboard Techniques**: Relies on bitwise operations for efficient board and piece handling.

## Key Features

- **Custom Evaluation**: Weighs piece values, checks for bishop pairs, counts doubled pawns, identifies open rook files, and evaluates passed pawns.  
- **Token-Efficient Code**: Strictly optimized to remain within the 500-token limit while still providing comprehensive chess heuristics.  
- **Strategic Heuristics**: Considers mobility and positional advantages such as phalanx formations and passers.

## Technical Details

- **Language**: C#  
- **API/Framework**: [ChessChallenge.API](https://github.com/SebLague/Chess-Challenge)  
- **Code Constraints**: 500-token limit, single file (`MyBot.cs`)
- **Evaluation Logic**: Combines piece-square tables, bitboard checks, and specialized heuristics for positional play.

## Development Process

1. **Design & Planning**: Determined which heuristics to include given the token constraint
2. **Implementation**: Wrote `MyBot.cs` to implement `IEvaluator`, carefully managing code size.  
3. **Testing**: Verified correctness and performance using the Chess Challenge framework.

## Current Status

- The bot runs within the ChessChallenge environment, adhering to the 500-token rule.  
- Work continues on refining heuristics and improving efficiency within the code limit.

## Repository Link

- **Challenge Reference**: [Sebastian Lague’s Chess Challenge](https://github.com/SebLague/Chess-Challenge)
