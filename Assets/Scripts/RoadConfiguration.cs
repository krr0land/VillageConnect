using System.Collections.Generic;

public static class RoadConfiguration
{
    // Determines which road prefab to place based on the surrounding tiles

    // 1 2 3
    // 4 . 5
    // 6 7 8

    // Representation: 12345678 where 0 is empty/obstacle/village, 1 is road

    public static readonly Dictionary<byte, (int, float)> configuration = new Dictionary<byte, (int, float)>
    {
        // case 1
        { 0b0000_0000, (1, 0) },

        // case 2
        { 0b0100_0000, (2, 180) },
        { 0b0001_0000, (2, 90) },
        { 0b0000_1000, (2, 270) },
        { 0b0000_0010, (2, 0) },

        // case 3
        { 0b0001_1000, (3, 90) },
        { 0b0100_0010, (3, 0) },

        // case 4
        { 0b0101_0000, (4, 180) },
        { 0b0100_1000, (4, 270) },
        { 0b0001_0010, (4, 90) },
        { 0b0000_1010, (4, 0) },

        // case 5
        { 0b0101_1000, (5, 180) },
        { 0b0100_1010, (5, 270) },
        { 0b0001_1010, (5, 0) },
        { 0b0101_0010, (5, 90) },

        // case 6
        { 0b0001_0110, (6, 90) },
        { 0b1101_0000, (6, 180) },
        { 0b0110_1000, (6, 270) },
        { 0b0000_1011, (6, 0) },


        // case 7
        { 0b0001_1111, (7, 0) },
        { 0b1101_0110, (7, 90) },
        { 0b0110_1011, (7, 270) },
        { 0b1111_1000, (7, 180) },

        // case 8
        { 0b0101_1111, (8, 180) },
        { 0b1101_1110, (8, 270) },
        { 0b0111_1011, (8, 90) },
        { 0b1111_1010, (8, 0) },

        // case 9
        { 0b0111_1111, (9, 90) },
        { 0b1101_1111, (9, 180) },
        { 0b1111_1011, (9, 0) },
        { 0b1111_1110, (9, 270) },

        // case 10
        { 0b0101_1010, (10, 0) },
        
        // case 11
        { 0b1111_1111, (11, 0) },
    };
}
