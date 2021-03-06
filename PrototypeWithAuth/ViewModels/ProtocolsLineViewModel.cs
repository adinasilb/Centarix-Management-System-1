using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ProtocolsLineViewModel
    {
        public int Index { get; set; }
        public Line Line { get; set; }
        public string LineNumberString { get; set; }
        public List<LineType> LineTypes { get; set; }
        public List<FunctionLine> Functions { get; set; }
        public AppUtility.ProtocolModalType ModalType { get; set; }
        public Guid UniqueGuid { get; set; }
        public bool IsDone { get; set; }
        public bool IsLast { get; set; }
        public LineChange LineChange { get; set;}
    }
}
