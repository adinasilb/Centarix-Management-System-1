using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    [Table("Lines")]
    public class Line : LineBase , IEquatable<Line>
    {
        public  Line ParentLine { get; set; }
        public bool IsTemporaryDeleted { get; set; }
        public IEnumerable<LineChange> LineChange { get; set; }
        public IEnumerable<FunctionLine> FunctionLines { get; set; }

        public bool Equals([AllowNull] Line other)
        {
            return this.LineID == other.LineID;
        }
    }
}
