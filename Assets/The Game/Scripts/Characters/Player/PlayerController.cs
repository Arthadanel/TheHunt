using UnityEngine;

/// <summary>
/// Управление движением персонажа.
/// </summary>
public class PlayerController : MonoBehaviour
{
	/// <summary>
	/// Скорость ходьбы.
	/// </summary>
	[SerializeField] private float speed = 2.0f;
	/// <summary>
	/// Уровень гравитации.
	/// </summary>
	[SerializeField] private float gravity = 20.0f;

	/// <summary>
	/// Звук шагов.
	/// </summary>
	[SerializeField] internal AudioSource walk;

	/// <summary>
	/// Вектор движения.
	/// </summary>
	private Vector3 moveDirection = Vector3.zero;
	/// <summary>
	/// Контроллер персонажа.
	/// </summary>
	private CharacterController controller;

	/// <summary>
	/// Сражается ли персонаж.
	/// </summary>
	private bool engaged;
	/// <summary>
	/// Сражается ли персонаж.
	/// </summary>
	public bool Engaged { get { return engaged; } set { engaged = value; } }

	/// <summary>
	/// Начальная инициализация. Присваивание начальных значений.
	/// </summary>
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		controller = GetComponent<CharacterController>();
		walk.volume = PlayerPrefs.GetFloat("sound_volume") * PlayerPrefs.GetFloat("master_volume");
		pitch = walk.pitch;
	}

	/// <summary>
	/// Высота звука.
	/// </summary>
	private float pitch;
	/// <summary>
	/// Кулдаун выносливости (для бега).
	/// </summary>
	bool spCooldown;

	/// <summary>
	/// Обновление состояния персонажа.
	/// </summary>
	void Update()
	{
		if (!engaged)
		{
			if (GetComponent<CharacterController>().isGrounded)
			{
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

				if (moveDirection.magnitude > Vector3.zero.magnitude)
				{
					if (!walk.isPlaying)
						walk.Play();
				}
				else
					walk.Stop();

				moveDirection = transform.TransformDirection(moveDirection);
				moveDirection = moveDirection * speed;

				if (!spCooldown && MazeLoader.mainPlayer.GetComponent<PlayerInfo>().SP > 0f && Input.GetKey(KeyCode.LeftShift))
				{
					moveDirection *= 1.5f;
					walk.pitch = pitch + 0.15f;
				}
				else
					walk.pitch = pitch;
			}

			// Apply gravity
			moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

			// Move the controller
			controller.Move(moveDirection * Time.deltaTime);

			if (Input.GetKeyDown(KeyCode.Escape))
				Cursor.lockState = CursorLockMode.None;
		}
		else
			walk.Stop();
	}

	/// <summary>
	/// Обновление некоторых параметров персонажа.
	/// </summary>
	private void FixedUpdate()
	{
		if (!engaged)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

			if (!spCooldown && moveDirection.magnitude > Vector3.zero.magnitude && MazeLoader.mainPlayer.GetComponent<PlayerInfo>().SP > 0f && Input.GetKey(KeyCode.LeftShift))
			{
				MazeLoader.mainPlayer.GetComponent<PlayerInfo>().SP -= 0.025f;
				if (MazeLoader.mainPlayer.GetComponent<PlayerInfo>().SP <= 0)
				{
					MazeLoader.mainPlayer.GetComponent<PlayerInfo>().SP = 0;
					if (!MazeLoader.mainPlayer.GetComponent<PlayerController>().Engaged)
						spCooldown = true;
				}
			}
			if (MazeLoader.mainPlayer.GetComponent<PlayerInfo>().SP > MazeLoader.mainPlayer.GetComponent<PlayerInfo>().maxSP * 0.5f)
				spCooldown = false;
		}
	}
}