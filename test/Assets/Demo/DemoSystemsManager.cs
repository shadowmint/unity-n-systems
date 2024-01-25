using System.Collections;
using System.Collections.Generic;
using Demo.Services.Concrete;
using N.Package.Bind;
using N.Package.GameSystems;
using UnityEngine;

namespace Demo
{
    public class DemoSystemsManager : SystemsManager
    {
        
        protected override IServiceModule Services()
        {
            return new DemoSystemsConfig();
        }
    }

    public class DemoSystemsConfig : IServiceModule
    {
        public void Register(ServiceRegistry registry)
        {
            registry.Register<LogService>();
        }
    }
}