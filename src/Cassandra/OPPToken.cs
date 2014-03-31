namespace Cassandra
{
    class OPPToken : IToken
    {
        private readonly byte[] _value;

        class OPPTokenFactory : TokenFactory
        {
            public override IToken Parse(string tokenStr)
            {
                return new OPPToken(System.Text.Encoding.UTF8.GetBytes(tokenStr));
            }

            public override IToken Hash(byte[] partitionKey)
            {
                return new OPPToken(partitionKey);
            }
        }

        public static readonly TokenFactory Factory = new OPPTokenFactory();


        private OPPToken(byte[] value)
        {
            this._value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null || (this.GetType() != obj.GetType()))
                return false;

            var other = obj as OPPToken;
            if (_value.Length != other._value.Length)
                return false;

            for (int i = 0; i < _value.Length; i++)
                if (_value[i] != other._value[i])
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var other = obj as OPPToken;
            for (int i = 0; i < _value.Length && i < other._value.Length; i++)
            {
                int a = (_value[i] & 0xff);
                int b = (other._value[i] & 0xff);
                if (a != b)
                    return a - b;
            }
            return 0;
        }
    }
}