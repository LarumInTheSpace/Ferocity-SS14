using Content.Server.Ferocity.Speech.Components;
using Content.Server.Speech;
using System.Text;

namespace Content.Server.Ferocity.Speech.EntitySystems
{
    public sealed class LatvianAccentSystem : EntitySystem
    {
        public override void Initialize()
        {
            SubscribeLocalEvent<LatvianAccentComponent, AccentGetEvent>(OnAccent);
        }

        public string Accentuate(string message)
        {
            var accentedMessage = new StringBuilder(message);

            for (var i = 0; i < accentedMessage.Length; i++)
            {
                var c = accentedMessage[i];

                accentedMessage[i] = c switch
                {
                    //Russian
                    'а' => 'ā',
                    'ч' => 'č',
                    'и' => 'ī',
                    'ж' => 'ž',
                    'к' => 'ķ',
                    'н' => 'ņ',
                    'у' => 'ū',
                    'з' => 'z',

                    'А' => 'Ā',
                    'Ч' => 'Č',
                    'И' => 'Ī',
                    'Ж' => 'Ž',
                    'К' => 'Ķ',
                    'Н' => 'Ņ',
                    'У' => 'Ū',
                    'З' => 'Z',
                    //English
                    'a' => 'ā',
                    'i' => 'ī',
                    'z' => 'ž',
                    'c' => 'č',
                    'k' => 'ķ',
                    'n' => 'ņ',
                    'u' => 'ū',

                    'A' => 'Ā',
                    'I' => 'Ī',
                    'Z' => 'Ž',
                    'C' => 'Č',
                    'K' => 'Ķ',
                    'N' => 'Ņ',
                    'U' => 'Ū',
                    _ => accentedMessage[i]
                };
            }

            return accentedMessage.ToString();
        }

        private void OnAccent(EntityUid uid, LatvianAccentComponent component, AccentGetEvent args)
        {
            args.Message = Accentuate(args.Message);
        }
    }
}
