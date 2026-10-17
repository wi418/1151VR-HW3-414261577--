
using UnityEngine;
using System.Collections;

public class RouletteController : MonoBehaviour
{
    public float maxSpeed = 1000f;
    public float acceleration = 500f;
    public float deceleration = 200f;

    private float currentSpeed;
    private bool isSpinning;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 保留空白鍵啟動功能
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSpin();
        }
    }

    // 給 UI 按鈕呼叫的公開函式
    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinWheel());
        }
    }

    IEnumerator SpinWheel()
    {
        isSpinning = true;
        currentSpeed = 0f;

        // 開始播放音效
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }

        // 加速
        while (currentSpeed < maxSpeed)
        {
            currentSpeed = Mathf.Min(
                currentSpeed + acceleration * Time.deltaTime,
                maxSpeed
            );

            transform.Rotate(0f, 0f, -currentSpeed * Time.deltaTime);
            yield return null;
        }

        // 維持最高速度 2 秒
        yield return new WaitForSeconds(2f);

        // 減速直到停止
        while (currentSpeed > 0f)
        {
            currentSpeed = Mathf.Max(
                currentSpeed - deceleration * Time.deltaTime,
                0f
            );

            transform.Rotate(0f, 0f, -currentSpeed * Time.deltaTime);
            yield return null;
        }

        // 停止音效
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        isSpinning = false;
    }
}
