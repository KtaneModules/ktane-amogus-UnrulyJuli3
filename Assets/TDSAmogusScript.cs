using KModkit;
using UnityEngine;

public class TDSAmogusScript : MonoBehaviour
{
	public KMBombModule Module;
	public KMBombInfo BombInfo;
	public KMAudio Audio;
	public TextMesh[] TextMeshes;
	public KMSelectable[] Buttons;

	private static int midcount;
	private int mid;
	private string[] serials;

	void Start()
	{
		mid = ++midcount;

		serials = new string[TextMeshes.Length];
		for (int i = 0; i < serials.Length; i++) serials[i] = i == 0 ? BombInfo.GetSerialNumber() : RandomSerial();
		serials.Shuffle();
		for (int i = 0; i < serials.Length; i++) TextMeshes[i].text = serials[i];

		for (int i = 0; i < Buttons.Length; i++) AssignButton(i);

		Debug.LogFormat("[amogus #{0}] Panels: {1}", mid, serials.Join(", "));
	}

	private bool solved;

	void AssignButton(int i)
	{
		KMSelectable button = Buttons[i];
		button.OnInteract += delegate
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, button.transform);
			button.AddInteractionPunch(1f);
			if (!solved)
			{
				Debug.LogFormat("[amogus #{0}] Pressed {1}. Expected {2}.", mid, serials[i], BombInfo.GetSerialNumber());
				if (serials[i] == BombInfo.GetSerialNumber())
				{
					Debug.LogFormat("[amogus #{0}] Module solved.", mid);
					solved = true;
					Module.HandlePass();
				}
				else
				{
					Debug.LogFormat("[amogus #{0}] Module striked.", mid);
					Module.HandleStrike();
				}
			}
			return false;
		};
	}

	private static readonly char[] serialChars = new char[35]
	{
		'A', 'B', 'C', 'D', 'E',
		'F', 'G', 'H', 'I', 'J',
		'K', 'L', 'M', 'N', 'E',
		'P', 'Q', 'R', 'S', 'T',
		'U', 'V', 'W', 'X', 'Z',
		'0', '1', '2', '3', '4',
		'5', '6', '7', '8', '9'
	};

	private string RandomSerial()
	{
		string number = string.Empty;
		for (int i = 0; i < 2; i++) number += serialChars[UnityEngine.Random.Range(0, serialChars.Length)];
		number += UnityEngine.Random.Range(0, 10);
		for (int i = 0; i < 2; i++) number += serialChars[UnityEngine.Random.Range(0, serialChars.Length - 10)];
		number += UnityEngine.Random.Range(0, 10);
		return number;
	}
}
