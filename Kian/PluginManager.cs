using Kian.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Kian
{
    internal class PluginManager<T> : IDisposable
    {
        private CompositionContainer Container;

        [ImportMany]
        public IEnumerable<IPlugin> Plugins
        {
            get;
            set;
        }

        public PluginManager(string path)
        {
            DirectoryCatalog directoryCatalog = new DirectoryCatalog(path);

            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog(directoryCatalog);

            // Create the CompositionContainer with all parts in the catalog (links Exports and Imports)
            Container = new CompositionContainer(catalog);

            //Fill the imports of this object
            Container.ComposeParts(this);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}


