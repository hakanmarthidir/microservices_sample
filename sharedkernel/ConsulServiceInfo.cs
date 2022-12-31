namespace sharedkernel
{
    public class ConsulServiceInfo
    {
        public string ServiceIp { get; set; }
        public int ServicePort { get; set; }
        public string ServiceName { get; set; }

        public override string ToString()
        {
            return $"{ServiceIp} {ServiceName} {ServicePort}";
        }
    }

    public class ConsulIdentityServiceInfo : ConsulServiceInfo{}
    public class ConsulCatalogServiceInfo : ConsulServiceInfo{}
    public class ConsulReviewServiceInfo : ConsulServiceInfo{}
    public class ConsulShelveServiceInfo : ConsulServiceInfo { }
}

