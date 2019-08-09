using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;



namespace WpfTetris.Models
{
    /// <summary>
    /// テトリミノの種類を表します。
    /// </summary>
    public enum TetriminoKind
    {
        /// <summary>
        /// <para>□■□□</para>
        /// <para>□■□□</para>
        /// <para>□■□□</para>
        /// <para>□■□□</para>
        /// </summary>
        I = 0,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■■□</para>
        /// <para>□■■□</para>
        /// <para>□□□□</para>
        /// </summary>
        O,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■■□</para>
        /// <para>■■□□</para>
        /// <para>□□□□</para>
        /// </summary>
        S,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■■□</para>
        /// <para>□□■■</para>
        /// <para>□□□□</para>
        /// </summary>
        Z,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■□□</para>
        /// <para>□■■■</para>
        /// <para>□□□□</para>
        /// </summary>
        J,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□□■□</para>
        /// <para>■■■□</para>
        /// <para>□□□□</para>
        /// </summary>
        L,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■□□</para>
        /// <para>■■■□</para>
        /// <para>□□□□</para>
        /// </summary>
        T,

        /// <summary>
        /// <para>⍰⍰⍰</para>
        /// <para>⍰⍰⍰</para>
        /// <para>⍰⍰⍰</para>
        /// </summary>
        RANDOM,

        /// <summary>
        /// <para>⍰⍰⍰</para>
        /// <para>⍰⍰⍰</para>
        /// <para>⍰⍰⍰</para>
        /// </summary>
        NEXTRANDOM,
    }



    /// <summary>
    /// テトリミノの種類の拡張機能を提供します。
    /// </summary>
    public static class TetriminoExtensions
    {
        private static Random rand = new Random();
        public static int[,] CurrentRandomPatternUp { get; set; }
        public static int[,] NextRandomPatternUp { get; set; }

        /// <summary>
        /// ブロックの色を取得します。
        /// </summary>
        /// <param name="self">テトリミノの種類</param>
        /// <returns>色</returns>
        public static Color BlockColor(this TetriminoKind self)
        {
            switch (self)
            {
                case TetriminoKind.I:   return Colors.LightBlue;
                case TetriminoKind.O:   return Colors.Yellow;
                case TetriminoKind.S:   return Colors.YellowGreen;
                case TetriminoKind.Z:   return Colors.Red;
                case TetriminoKind.J:   return Colors.Blue;
                case TetriminoKind.L:   return Colors.Orange;
                case TetriminoKind.T:   return Colors.Purple;
                case TetriminoKind.RANDOM: return Colors.Violet;
                case TetriminoKind.NEXTRANDOM: return Colors.Coral;
            }
            throw new InvalidOperationException("Unknown Tetrimino");
        }


        /// <summary>
        /// ブロックの初期位置を取得します。
        /// </summary>
        /// <param name="self">テトリミノの種類</param>
        /// <returns>初期位置</returns>
        public static Position InitialPosition(this TetriminoKind self)
        {
            int length = 0;
            switch (self)
            {
                case TetriminoKind.I:   length = 4; break;
                case TetriminoKind.O:   length = 2; break;
                case TetriminoKind.S:
                case TetriminoKind.Z:
                case TetriminoKind.J:
                case TetriminoKind.L:
                case TetriminoKind.T:   length = 3; break;
                case TetriminoKind.RANDOM: length = 3; break;
                case TetriminoKind.NEXTRANDOM: length = 3; break;
                default:    throw new InvalidOperationException("Unknown Tetrimino");
            }

            var row = -length;
            var column = (Field.ColumnCount - length) / 2;
            return new Position(row, column);
        }


        /// <summary>
        /// 指定されたテトリミノの種類と方向に一致したブロックを生成します。
        /// </summary>
        /// <param name="self">テトリミノの種類</param>
        /// <param name="offset">絶対座標への移動分</param>
        /// <param name="direction">方向</param>
        /// <returns>ブロックのコレクション</returns>
        public static IReadOnlyList<Block> CreateBlock(this TetriminoKind self, Position offset, Direction direction = Direction.Up)
        {
            //--- ブロック形状をビットで表現
            //--- ベタ書きだけど、これが最も分かりやすく高速
            int[,] pattern = null;
            switch (self)
            {
                #region I
                case TetriminoKind.I:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 0, 0, 0 },
                                { 1, 1, 1, 1 },
                                { 0, 0, 0, 0 },
                                { 0, 0, 0, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 0, 0, 0 },
                                { 0, 0, 0, 0 },
                                { 1, 1, 1, 1 },
                                { 0, 0, 0, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region O
                case TetriminoKind.O:
                    pattern = new int[,]
                    {
                        { 1, 1 },
                        { 1, 1 },
                    };
                    break;
                #endregion

                #region S
                case TetriminoKind.S:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 1 },
                                { 1, 1, 0 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 0, 1 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 0, 1, 1 },
                                { 1, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 1, 0, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region Z
                case TetriminoKind.Z:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 1, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 0, 1 },
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 1 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                                { 1, 0, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region J
                case TetriminoKind.J:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 1, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 1 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region L
                case TetriminoKind.L:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 0, 1 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 1, 0, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region T
                case TetriminoKind.T:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region RANDOM
                case TetriminoKind.RANDOM:
                    switch(direction)
                    {
                        case Direction.Up:
                            pattern = CurrentRandomPatternUp;
                            break;
                        case Direction.Right:
                            pattern = RotatePatternRight(CurrentRandomPatternUp);
                            break;
                        case Direction.Down:
                            pattern = RotatePatternRight(CurrentRandomPatternUp);
                            pattern = RotatePatternRight(pattern);
                            break;
                        case Direction.Left:
                            pattern = RotatePatternRight(CurrentRandomPatternUp);
                            pattern = RotatePatternRight(pattern);
                            pattern = RotatePatternRight(pattern);
                            break;
                    }
                    break;
                #endregion


                #region NEXTRANDOM
                case TetriminoKind.NEXTRANDOM:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = NextRandomPatternUp;
                            break;
                        case Direction.Right:
                            pattern = RotatePatternRight(NextRandomPatternUp);
                            break;
                        case Direction.Down:
                            pattern = RotatePatternRight(NextRandomPatternUp);
                            pattern = RotatePatternRight(pattern);
                            break;
                        case Direction.Left:
                            pattern = RotatePatternRight(NextRandomPatternUp);
                            pattern = RotatePatternRight(pattern);
                            pattern = RotatePatternRight(pattern);
                            break;
                    }
                    break;
                    #endregion
            }

            //--- どれにも当てはまらなかった
            if (pattern == null)
                throw new InvalidOperationException("Unknown Tetrimino");

            //--- ビットが立っている部分にブロックを作成
            var color = self.BlockColor();
            return  Enumerable.Range(0, pattern.GetLength(0))
                    .SelectMany(r => Enumerable.Range(0, pattern.GetLength(1)).Select(c => new Position(r, c)))
                    .Where(x => pattern[x.Row, x.Column] != 0)  //--- bit が立っているところ
                    .Select(x => new Position(x.Row + offset.Row, x.Column + offset.Column))  //--- 絶対座標変換
                    .Select(x => new Block(color, x))
                    .ToArray();
        }

        private static int[,] RotatePatternRight(int[,] currentPatternUp)
        {
            int[,] retVal = new int[3, 3];
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    retVal[i, j] = currentPatternUp[3 - j - 1, i];
                }
            }
            return retVal;
        }

        public static void SetRandomPattern()
        {
            CurrentRandomPatternUp = new int[3, 3];
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    CurrentRandomPatternUp[i, j] = rand.Next(0, 2);
                }
            }
            CurrentRandomPatternUp = MakePatternValid(CurrentRandomPatternUp);
        }
        public static void SetNextRandomPattern()
        {
            NextRandomPatternUp = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    NextRandomPatternUp[i, j] = rand.Next(0, 2);
                }
            }
            NextRandomPatternUp = MakePatternValid(NextRandomPatternUp);
        }

        private static int[,] MakePatternValid(int[,] pattern)
        {
            List<int[]> safeLocations = new List<int[]>();
            bool firstOne = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(pattern[i,j] == 1 && firstOne == false)
                    {
                        firstOne = true;
                        safeLocations.AddRange(CheckPoint(i, j, pattern, safeLocations));
                    }
                    else if(pattern[i,j] == 1 && !safeLocations.Any(x => x.SequenceEqual(new int[] { i, j })))
                    {
                        pattern[i, j] = 0;
                    }
                }
            }
            return pattern;
        }

        private static IEnumerable<int[]> CheckPoint(int i, int j, int[,] pattern, List<int[]> safeLocations)
        {
            //List<int[]> tempSafeLocations = new List<int[]>();
            if (safeLocations.Any(x => x.SequenceEqual(new int[] { i, j })))
            {
                //Returing safe locations wont work, make a new list
                return new List<int[]>();
            }
            safeLocations.Add(new int[] { i, j });
            if(i != 0)
            {
                if(pattern[i - 1, j] == 1 )
                {
                    safeLocations.AddRange(CheckPoint(i - 1, j, pattern, safeLocations));
                }
            }
            if (j != 0)
            {
                if (pattern[i, j - 1] == 1)
                {
                    safeLocations.AddRange(CheckPoint(i, j - 1, pattern, safeLocations));
                }
            }
            if (i != 2)
            {
                if (pattern[i + 1, j] == 1)
                {
                    safeLocations.AddRange(CheckPoint(i + 1, j, pattern, safeLocations));
                }
            }
            if (j != 2)
            {
                if (pattern[i, j + 1] == 1)
                {
                    safeLocations.AddRange(CheckPoint(i, j + 1, pattern, safeLocations));
                }
            }
            return safeLocations;
        }
    }
}