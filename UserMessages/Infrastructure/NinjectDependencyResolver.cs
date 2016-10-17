using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using UserMessages.Infrastructure.Interfaces;
using UserMessages.Infrastructure;

namespace EssentialTools.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IParser>().To<Parser>();
            kernel.Bind<IDownloader>().To<Downloader>();
            kernel.Bind<IHtmlParser>().To<HtmlAgilityParser>();
        }
    }
}