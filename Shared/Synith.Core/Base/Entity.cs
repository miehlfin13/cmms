using Synith.Core.Enums;

namespace Synith.Core.Base;
public abstract class Entity
{
    public int Id { get; set; }
    public GenericStatus Status { get; set; } = GenericStatus.Active;
    public DateTime? InactiveDate { get; set; }
}
