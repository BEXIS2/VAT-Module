using BExIS.Security.Entities.Objects;
using BExIS.Security.Services.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BExIS.Modules.VAT.UI.Helpers
{
    public class VATSeedDataGenerator : IDisposable
    {
        public void GenerateSeedData()
        {
            using (FeatureManager featureManager = new FeatureManager())
            using (OperationManager operationManager = new OperationManager())
            {
                //Features
                List<Feature> features = featureManager.FeatureRepository.Get().ToList();

                Feature VAT = features.FirstOrDefault(f => f.Name.Equals("VAT"));
                if (VAT == null) VAT = featureManager.Create("VAT", "Interface to a tool that provides visualize, analyze and transform functions");

                Feature AdminFeature = features.FirstOrDefault(f =>
                    f.Name.Equals("Admin") &&
                    f.Parent != null &&
                    f.Parent.Id.Equals(VAT.Id));

                Feature DisplayFeature = features.FirstOrDefault(f =>
                    f.Name.Equals("Display") &&
                    f.Parent != null &&
                    f.Parent.Id.Equals(VAT.Id));

                Feature API = features.FirstOrDefault(f =>
                    f.Name.Equals("API") &&
                    f.Parent != null &&
                    f.Parent.Id.Equals(VAT.Id));


                //Operations
                operationManager.Create("VAT", "Admin", "*", AdminFeature);
                operationManager.Create("VAT", "Home", "*", DisplayFeature);

                operationManager.Create("VAT", "Config", "*", DisplayFeature);
                operationManager.Create("API", "GeoRef", "*", API);

            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
