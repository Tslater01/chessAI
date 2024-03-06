using ChessChallenge.API;
using System.Linq
using System;

/*
500 tokens
By Tyler Slater
*/

public class Evaluator : IEvaluator{
    private static readonly int[] PieceValues = { 77, 302, 310, 434, 890, 0, 109, 331, 335, 594, 1116, 0}, // Piece Values
                                  UnpackedPestoTables = new[]{
                                    59445390105436474986072674560m, 70290677894333901267150682880m, 71539517137735599738519086336m, 78957476706409475571971323392m, 76477941479143404670656189696m, 78020492916263816717520067072m, 77059410983631195892660944640m, 61307098105356489251813834752m,
                                    77373759864583735626648317994m, 3437103645554060776222818613m, 5013542988189698109836108074m, 2865258213628105516468149820m, 5661498819074815745865228343m, 8414185094009835055136457260m, 7780689186187929908113377023m, 2486769613674807657298071274m,
                                    934589548775805732457284597m, 4354645360213341838043912961m, 8408178448912173986754536726m, 9647317858599793704577609753m, 9972476475626052485400971547m, 9023455558428990305557695533m, 9302688995903440861301845277m, 4030554014361651745759368192m,
                                    78006037809249804099646260205m, 5608292212701744542498884606m, 9021118043939758059554412800m, 11825811962956083217393723906m, 11837863313235587677091076880m, 11207998775238414808093699594m, 9337766883211775102593666830m, 4676129865778184699670239740m,
                                    75532551896838498151443462373m, 3131203134016898079077499641m, 8090231125077317934436125943m, 11205623443703685966919568899m, 11509049675918088175762150403m, 9025911301112313205746176509m, 6534267870125294841726636036m, 3120251651824756925472439792m,
                                    74280085839011331528989207781m, 324048954150360030097570806m, 4681017700776466875968718582m, 7150867317927305549636569078m, 7155688890998399537110584833m, 5600986637454890754120354040m, 1563108101768245091211217423m, 78303310575846526174794479097m,
                                    70256775951642154667751105509m, 76139418398446961904222530552m, 78919952506429230065925355250m, 2485617727604605227028709358m, 3105768375617668305352130555m, 1225874429600076432248013062m, 76410151742261424234463229975m, 72367527118297610444645922550m,
                                    64062225663112462441888793856m, 67159522168020586196575185664m, 71185268483909686702087266048m, 75814236297773358797609495296m, 69944882517184684696171572480m, 74895414840161820695659345152m, 69305332238573146615004392448m, 63422661310571918454614119936m,
                                  }.SelectMany(packedTable =>
                                  decimal.GetBits(packedTable).SelectMany(BitConverter.GetBytes)
                                            .Select((square, index) => (int)((sbyte)square * 1.461) + PieceValues[index % 12])
                                        .ToArray()
                                  ).ToArray(); // Tyrant PSTS
    ulong[] val = {0, 0, 1514915904008557569, 18372690887341639933, 505817156140206860, 18086434177696465413, 1138024818088203, 577865971130957834}; // Passers
    int g(int n) => (sbyte)val[n / 8] >> (n % 8 * 8); // Extract from passers
public int Evaluate(Board board, Timer timer){
        int mg = 0, eg = 0, gp = 0, side = 2, piece, square;
        for (; --side >= 0; mg = -mg, eg = -eg)
            for (piece = 6; --piece >= 0;)
                for (ulong mask = board.GetPieceBitboard((PieceType)piece + 1, side > 0); mask != 0;){
                    gp += 0x00042110 >> piece * 4 & 0x0F; // Gamephase

                    // Material and square evaluation
                    square = BitboardHelper.ClearAndGetIndexOfLSB(ref mask) ^ 56 * side;
                    mg += UnpackedPestoTables[square * 16 + piece];
                    eg += UnpackedPestoTables[square * 16 + piece + 6];

                    
                    // Bishop Pair
                    if (piece == 2 && mask != 0){
                        mg += 35;
                        eg += 55;
                    }

                    if (piece == 0){
                        // Doubled Pawns
                        if ((0x101010101010101UL << (square & 7) & mask) > 0){
                            mg += 3;
                            eg -= 23;
                        }
                        
                        // Phalanx
                        int bonus = BitboardHelper.GetNumberOfSetBits(mask & (mask << 1));
                        mg += 4*bonus;
                        eg += 2*bonus;

                        // Passers
                        if(square / 8 < 4 && (0x101010101010101UL << (square & 7) & board.GetPieceBitboard(PieceType.Pawn, side > 1)) == 0){
                            mg += g(2 * square);
                            eg += g(2 * square + 1);
                        }
                    }
                    
                    // Open rook file
                    if (piece == 3 && (0x101010101010101UL << (square & 7) & board.GetPieceBitboard(PieceType.Pawn, side > 0)) == 0){
                        mg += 38;
                        eg += 13;
                    }

                    // Mobility
                    if (piece >= 2 && piece <= 4){
                        int bonus = BitboardHelper.GetNumberOfSetBits(BitboardHelper.GetPieceAttacks((PieceType)piece + 1, new Square(square ^ 56 * side), board, side > 0));
                        mg += bonus;
                        eg += bonus * 2;
                    }
                    
                }
        return (mg * gp + eg * (24 - gp)) / (board.IsWhiteToMove ? 24 : -24);
    }
}
