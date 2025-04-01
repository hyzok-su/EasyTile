using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tile
{
    public class Tile3d
    {
        public string Name { get; }
        public object Attribute { get; set; }
        public Socket Socket0 { get; internal set; }
        public Socket Socket1 { get; internal set; }
        public Socket Socket2 { get; internal set; }
        public Socket Socket3 { get; internal set; }
        public Socket Socket4 { get; internal set; }
        public Socket Socket5 { get; internal set; }
        public sbyte[] Matrix { get; internal set; }
        public Tile3d(string name, Socket xn, Socket xp, Socket yn, Socket yp, Socket zn, Socket zp)
        {
            Name = name;
            Socket0 = xn;
            Socket1 = xp;
            Socket2 = yn;
            Socket3 = yp;
            Socket4 = zn;
            Socket5 = zp;
            Matrix = MatrixHelper.None;
        }
        public Tile3d RotateZ()
        {
            var output = new Tile3d(this.Name, this.Socket3, this.Socket2, this.Socket0, this.Socket1,
                this.Socket4.RotateShift().RotateShift().RotateShift(), this.Socket5.RotateShift());
            output.Matrix = MatrixHelper.MultMat(MatrixHelper.RotZ,this.Matrix);
            return output;
        }
        public Tile3d FlipX()
        {
            var output = new Tile3d(this.Name, this.Socket1.FlipSwitch(FlipType.Horizontal), this.Socket0.FlipSwitch(FlipType.Horizontal),
                this.Socket2.FlipSwitch(FlipType.Horizontal), this.Socket3.FlipSwitch(FlipType.Horizontal), 
                this.Socket4.FlipSwitch(FlipType.Horizontal), this.Socket5.FlipSwitch(FlipType.Horizontal));
            output.Matrix = MatrixHelper.MultMat(MatrixHelper.MirX, this.Matrix);
            return output;
        }

        public override bool Equals(object obj)
        {
            return obj is Tile3d d &&
                   Name == d.Name &&
                   EqualityComparer<Socket>.Default.Equals(Socket0, d.Socket0) &&
                   EqualityComparer<Socket>.Default.Equals(Socket1, d.Socket1) &&
                   EqualityComparer<Socket>.Default.Equals(Socket2, d.Socket2) &&
                   EqualityComparer<Socket>.Default.Equals(Socket3, d.Socket3) &&
                   EqualityComparer<Socket>.Default.Equals(Socket4, d.Socket4) &&
                   EqualityComparer<Socket>.Default.Equals(Socket5, d.Socket5);
        }

        public override int GetHashCode()
        {
            int hashCode = 1034992139;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Socket>.Default.GetHashCode(Socket0);
            hashCode = hashCode * -1521134295 + EqualityComparer<Socket>.Default.GetHashCode(Socket1);
            hashCode = hashCode * -1521134295 + EqualityComparer<Socket>.Default.GetHashCode(Socket2);
            hashCode = hashCode * -1521134295 + EqualityComparer<Socket>.Default.GetHashCode(Socket3);
            hashCode = hashCode * -1521134295 + EqualityComparer<Socket>.Default.GetHashCode(Socket4);
            hashCode = hashCode * -1521134295 + EqualityComparer<Socket>.Default.GetHashCode(Socket5);
            return hashCode;
        }

        public static bool operator ==(Tile3d t1, Tile3d t2)
        {
            return t1.Name == t2.Name
                && t1.Socket0 == t2.Socket0
                && t1.Socket1 == t2.Socket1
                && t1.Socket2 == t2.Socket2
                && t1.Socket3 == t2.Socket3
                && t1.Socket4 == t2.Socket4
                && t1.Socket5 == t2.Socket5;
        }
        public static bool operator !=(Tile3d t1, Tile3d t2)
        {
            return !(t1 == t2);
        }
    }
    public class Socket
    {
        public string Name { get; }
        public bool Flip { get; internal set; }
        public int Rotate { get; internal set; }
        public SocketFamily Family { get; }
        public Sym Achiral { get; }
        public bool CentroSym{ get; }
        public bool QuadSym { get; }

        public static bool operator == (Socket s1, Socket s2)
        {
            return s1.Name == s2.Name && s1.Flip == s2.Flip && s1.Rotate == s2.Rotate;
        }
        public static bool operator !=(Socket s1, Socket s2)
        {
            return !(s1 == s2);
        }
        public Socket(string name, bool flip, int rotate, SocketFamily family)
        {
            Name = name;
            Flip = flip;
            Rotate = rotate;
            Family = family;
            switch (family)
            {
                case SocketFamily.Fiddler: 
                    Achiral = Sym.NoSym; CentroSym = false; QuadSym = false; break;
                case SocketFamily.Balancer: 
                    Achiral = Sym.Sym; CentroSym = false; QuadSym = false; break;
                case SocketFamily.Slash: 
                    Achiral = Sym.SlashSym; CentroSym = false; QuadSym = false; break;
                case SocketFamily.Compass: 
                    Achiral = Sym.NoSym; CentroSym = false; QuadSym = false; break;
                case SocketFamily.Bi_edge: 
                    Achiral = Sym.Sym; CentroSym = true; QuadSym = false; break;
                case SocketFamily.Bi_corner: 
                    Achiral = Sym.SlashSym; CentroSym = true; QuadSym = false; break;
                case SocketFamily.Octopus: 
                    Achiral = Sym.NoSym; CentroSym = true; QuadSym = true; break;
                case SocketFamily.Map: 
                    Achiral = Sym.Sym; CentroSym = true; QuadSym = true; break;
            }

        }

        public Socket RotateShift()
        {
            return new Socket(this.Name, this.Flip, this.Rotate + 1, this.Family).Compact();
        }
        public Socket FlipSwitch( FlipType type)
        {
            int rot = this.Rotate;
            bool f = !this.Flip;
            switch (type)
            {
                case FlipType.Horizontal:
                    rot = (4 - rot) % 4; break;
                case FlipType.Vertical:
                    rot = (6 - rot) % 4; break;
            }
            return new Socket(this.Name, f, rot, this.Family).Compact();
        }
        public Socket Compact()
        {
            int rot = this.Rotate;
            bool f = this.Flip;
            if (this.Achiral == Sym.SlashSym)
            {
                if (f) { f = false; rot++; }
            }
            else if (this.Achiral == Sym.Sym)
            {
                if (f) f = false;
            }
            rot %= 4;
            if (this.QuadSym){ rot = 0; }
            else if (this.CentroSym){ rot %= 2; }
            this.Rotate = rot; this.Flip = f;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is Socket socket &&
                   Name == socket.Name &&
                   Flip == socket.Flip &&
                   Rotate == socket.Rotate;
        }

        public override int GetHashCode()
        {
            int hashCode = -1912673274;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Flip.GetHashCode();
            hashCode = hashCode * -1521134295 + Rotate.GetHashCode();
            return hashCode;
        }
    }
    public class SocketLib
    {
        private Dictionary<Socket, HashSet<Socket>> _socketPairs;
        public SocketLib()
        {
            this._socketPairs = new Dictionary<Socket, HashSet<Socket>>();
        }
        public void AddAllPairs(Socket start, Socket end)
        {
            var startf = start.FlipSwitch(FlipType.Horizontal);
            var endf = end.FlipSwitch(FlipType.Horizontal);
            AddPair(start, end); 
            AddPair(startf, endf);
            AddPair(start.RotateShift(), end.RotateShift().RotateShift().RotateShift()); 
            AddPair(startf.RotateShift(), endf.RotateShift().RotateShift().RotateShift());
            AddPair(start.RotateShift().RotateShift(), end.RotateShift().RotateShift()); 
            AddPair(startf.RotateShift().RotateShift(), endf.RotateShift().RotateShift());
            AddPair(start.RotateShift().RotateShift().RotateShift(), end.RotateShift()); 
            AddPair(startf.RotateShift().RotateShift().RotateShift(), endf.RotateShift());
        }
        public void AddPair(Socket start, Socket end)
        {
            if (this._socketPairs.ContainsKey(start))
            {
                if (!this._socketPairs[start].Contains(end))
                {
                    this._socketPairs[start].Add(end);
                }
            }
            else
            {
                this._socketPairs.Add(start, new HashSet<Socket> { end });
            }
        }
        public bool CheckPair(Socket start, Socket end)
        {
            return this._socketPairs.ContainsKey(start) && this._socketPairs[start].Contains(end);
        }
    }
    public enum SocketFamily
    {
        Fiddler,
        Balancer,
        Slash,
        Compass,
        Bi_edge,
        Bi_corner,
        Octopus,
        Map
    }
    public enum Sym
    {
        NoSym,
        Sym,
        SlashSym
    }
    public enum FlipType
    {
        Horizontal,Vertical
    }
    internal static class MatrixHelper
    {
        internal static sbyte[] None { get; } = { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
        internal static sbyte[] RotZ { get; } = { 0, -1, 0, 1, 0, 0, 0, 0, 1 };
        internal static sbyte[] RotY { get; } = { 0, 0, -1, 0, 1, 0, 1, 0, 0 };
        internal static sbyte[] RotX { get; } = { 1, 0, 0, 0, 0, -1, 0, 1, 0 };
        internal static sbyte[] MirX { get; } = { -1, 0, 0, 0, 1, 0, 0, 0, 1 };
        internal static sbyte[] MirY { get; } = { 1, 0, 0, 0, -1, 0, 0, 0, 1 };
        internal static sbyte[] MirZ { get; } = { 1, 0, 0, 0, 1, 0, 0, 0, -1 };

        [MethodImpl(256)]
        internal static sbyte[] MultMat(sbyte[] matrixA, sbyte[] matrixB)
        {
            sbyte[] output = new sbyte[9];
            sbyte temp;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    temp = 0;
                    for (int n = 0; n < 3; n++)
                    {
                        temp += (sbyte)(matrixA[i * 3 + n] * matrixB[n * 3 + j]);
                    }
                    output[i * 3 + j] = temp;
                }
            }
            return output;
        }
    }
}

