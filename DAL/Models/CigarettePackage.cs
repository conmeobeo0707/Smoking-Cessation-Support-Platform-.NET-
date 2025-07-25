using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CigarettePackage
    {
        public int CigaretteId { get; set; }
        public string CigaretteName { get; set; }
        public double Price { get; set; }
        public string Brand { get; set; }
        public string NicoteneStrength { get; set; }
        public string Flavor { get; set; }
        public int SticksPerPack { get; set; }
        public double NicotineMg { get; set; }
    }
}
