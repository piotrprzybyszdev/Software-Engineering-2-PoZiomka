﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Admin
{
	public class Admin
	{
		private int Id;
		public string Email;
		private List<Privilege> Privileges = new List<Privilege>();
	}
}
