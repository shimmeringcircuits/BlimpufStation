using Content.Shared.Body.Components;
using Content.Shared._Blimpuf.Medical.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared._Starlight.Medical.Body.Components;
using Content.Shared._Starlight.Medical.Body.Systems;
using Content.Shared._Starlight.Medical.Surgery.Components;

namespace Content.Server._Blimpuf.Medical.Body.Systems;

public sealed partial class DipsomaniaSystem : EntitySystem
{
    private const string DipsomaniaMetabolizerType = "Dipsomania";

    [Dependency] private SharedBodySystem _body = default!;
    [Dependency] private MetabolizerSystem _metabolizer = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<DipsomaniaComponent, ComponentStartup>(OnDipsomaniaStartup);
    }

    private void OnDipsomaniaStartup(Entity<DipsomaniaComponent> ent, ref ComponentStartup args)
    {
        if (!TryComp<BodyComponent>(ent, out var body)
            || !_body.TryGetOrgansWithComponent<OrganStomachComponent>((ent.Owner, body), out var stomachs)
            || !_body.TryGetOrgansWithComponent<OrganLiverComponent>((ent.Owner, body), out var livers)
            )
            return;

        var stomach = stomachs[0];
        var liver = livers[0];
        if (!TryComp<MetabolizerComponent>(stomach, out var stomachMetabolizer)
            || !TryComp<MetabolizerComponent>(liver, out var liverMetabolizer)
            )
            return;

        _metabolizer.TryAddMetabolizerType((stomach, stomachMetabolizer), DipsomaniaMetabolizerType);
        _metabolizer.TryAddMetabolizerType((liver, liverMetabolizer), DipsomaniaMetabolizerType);
        EnsureComp<DipsomaniaCarrierComponent>(stomach);
        EnsureComp<DipsomaniaCarrierComponent>(liver);
        RemComp<DipsomaniaComponent>(ent);
    }
}
