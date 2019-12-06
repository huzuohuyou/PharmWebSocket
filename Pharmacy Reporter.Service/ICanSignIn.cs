using Pharmacy_Reporter.Entity;
using Pharmacy_Reporter.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharmacy_Reporter.Service
{
    public interface ICanSignIn
    {
        SigninStrip SignIn(PharmacyReporterManager orm);
    }
}
