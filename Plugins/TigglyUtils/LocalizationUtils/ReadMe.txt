LocalizationUtils

Scripts for handling the localization, along with the Localization script in Assets/NGUI.

The LocalizableAudioClip is used to play audio clips depending on the language chosen.
The localizable audio clip is serializable, so it will show in your editor.

1) Place the Audio files in Resources folder as such: Resources/<Language_Key>/<path_to_file>
2) Set the LocalizableAudioClip's key to <path_to_file>
3) Play the sound by calling the extension method AudioSource.PlayOneShotController(LocalizableAudioClip, AudioSourceType)

SecondaryLocalization handles a second language for bilingual apps as Stamp. For now it is basically a duplicate of the primary Localization class, but should be better integrated when we have time.

Dependencies: NGUI