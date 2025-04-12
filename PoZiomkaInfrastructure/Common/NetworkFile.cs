using PoZiomkaDomain.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaInfrastructure.Common;
public class NetworkFile(MemoryStream stream): IFile
{
	MemoryStream stream = stream;
	public Stream Stream => stream;
}

