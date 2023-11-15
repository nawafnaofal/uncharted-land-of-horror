using System.Collections;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    // Nilai orthographic kamera awal sebelum guncangan
    public float originalOrthoValue = 6f;
    // Nilai orthographic kamera setelah guncangan
    public float endOrthoValue = 5.5f;
    // Durasi guncangan
    public float duration;
    // Magnitudo guncangan
    public float magnitude;
    // Penguatan amplitudo guncangan
    public float ampGain = 0.4f;
    // Referensi ke komponen kamera virtual Cinemachine
    CinemachineVirtualCamera cam;
    // Referensi ke komponen perlin noise Cinemachine
    CinemachineBasicMultiChannelPerlin camNoise;

    private void Start()
    {
        // Mencari objek pemain dalam scene
        Player player = FindObjectOfType<Player>();
        // Menghubungkan metode Shake() dengan event DamageTaken pada pemain
        player.DamageTaken += Shake;
        // Mendapatkan komponen CinemachineVirtualCamera pada objek ini
        cam = GetComponent<CinemachineVirtualCamera>();
        // Mendapatkan komponen CinemachineBasicMultiChannelPerlin dari kamera
        camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        // Memulai guncangan kamera jika objek ini aktif
        if (isActiveAndEnabled)
            StartCoroutine(CamShake());
    }

    public IEnumerator CamShake()
    {
        float t = 0;
        camNoise.m_AmplitudeGain = ampGain;
        // Punch kamera ke dalam
        while (cam.m_Lens.OrthographicSize != endOrthoValue)
        {
            // Mengubah nilai orthographic kamera secara berangsur
            cam.m_Lens.OrthographicSize = Mathf.Lerp(originalOrthoValue, endOrthoValue, t);
            t += Time.deltaTime * (1 / duration);
            yield return null;
        }

        t = 0;

        // Mengembalikan nilai orthographic kamera ke nilai awal
        while (cam.m_Lens.OrthographicSize != originalOrthoValue)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(endOrthoValue, originalOrthoValue, t);
            t += Time.deltaTime * (1 / duration);
            yield return null;
        }
        camNoise.m_AmplitudeGain = 0;
    }

    private void OnDisable()
    {
        // Mengatur nilai orthographic kamera dan amplitudo noise kamera menjadi nilai awal saat objek ini nonaktif
        cam.m_Lens.OrthographicSize = originalOrthoValue;
        camNoise.m_AmplitudeGain = 0;
    }
}
