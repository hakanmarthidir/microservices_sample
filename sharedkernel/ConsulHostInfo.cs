namespace sharedkernel
{
    public class ConsulHostInfo
    {
        public string ConsulHost { get; set; }
        public int ConsulPort { get; set; }

        public override string ToString()
        {
            return $"{ConsulHost} {ConsulPort}";
        }
    }
}

