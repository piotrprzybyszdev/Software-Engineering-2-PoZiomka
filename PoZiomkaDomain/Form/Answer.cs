using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Form
{
    public class Answer
    {
        public int Id;
        public List<ChoosablePreference> choosablePreferences = new List<ChoosablePreference>();
		public List<ObligatoryPrefernce> obligatoryPrefernces = new List<ObligatoryPrefernce>();
	}
}
