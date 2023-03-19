using Robust.Shared.Prototypes;

namespace Content.Server.Ferocity.FluffSlot;

[Prototype("fluffSlot")]
public sealed class FluffSlotPrototype : IPrototype
{
    [IdDataField] public string ID { get; } = default!;

    [DataField("item", required: true)]
    public string Item = default!;
}