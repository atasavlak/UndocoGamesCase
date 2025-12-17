using UnityEngine;
using System.Collections;

// Submarine mini oyununda oyuncu kontrolünü,
// hareket, dönüş, çarpışma ve görsel idle bobbing davranışlarını yöneten sınıf.

public class SubmarineController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float smoothTime = 0.15f;

    [Header("Rotation")]
    public float baseZRotation = 90f;
    public float pitchAngle = 30f;
    public float rotationSpeed = 10f;

    [Header("Knockback")]
    public float knockbackForce = 6f;
    public float knockbackDuration = 0.2f;

    [Header("Idle Bobbing (Visual Only)")]
    public Transform visual;
    public float bobAmplitude = 0.08f;
    public float bobFrequency = 1.1f;
    public float tiltAmplitude = 2f;
    public float bobResetSpeed = 6f;

    private Rigidbody rb;
    private Vector3 velocityRef;

    private float currentYRotation = 0f;
    private float currentZRotation = 90f;
    private bool isKnockbacked = false;

    // Bobbing için başlangıç referansları
    private Vector3 visualStartLocalPos;
    private Quaternion visualStartLocalRot;
    private float bobTimer = 0f;

    // Rigidbody ve görsel başlangıç değerlerini ayarlar
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (visual != null)
        {
            visualStartLocalPos = visual.localPosition;
            visualStartLocalRot = visual.localRotation;
        }
    }

    // Fizik tabanlı hareket ve dönüş hesaplamalarını yapar
    private void FixedUpdate()
    {
        if (isKnockbacked)
            return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 targetVelocity = new Vector3(h, v, 0f) * moveSpeed;

        rb.velocity = Vector3.SmoothDamp(
            rb.velocity,
            targetVelocity,
            ref velocityRef,
            smoothTime
        );

        HandleRotation(h, v);
    }

    // Görsel idle bobbing efektini günceller
    private void Update()
    {
        HandleIdleBobbing();
    }

    // Girişlere göre denizaltının dönüşünü ayarlar
    private void HandleRotation(float horizontalInput, float verticalInput)
    {
        if (horizontalInput > 0f)
            currentYRotation = 0f;
        else if (horizontalInput < 0f)
            currentYRotation = -180f;

        if (verticalInput > 0f)
            currentZRotation = baseZRotation - pitchAngle;
        else if (verticalInput < 0f)
            currentZRotation = baseZRotation + pitchAngle;
        else
            currentZRotation = baseZRotation;

        Quaternion targetRotation = Quaternion.Euler(
            0f,
            currentYRotation,
            currentZRotation
        );

        rb.MoveRotation(
            Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            )
        );
    }

    // Hareket yokken bobbing, hareket varken reset davranışını uygular
    private void HandleIdleBobbing()
    {
        if (visual == null)
            return;

        bool isMoving = rb.velocity.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            bobTimer = 0f;

            visual.localPosition = Vector3.Lerp(
                visual.localPosition,
                visualStartLocalPos,
                bobResetSpeed * Time.deltaTime
            );

            visual.localRotation = Quaternion.Slerp(
                visual.localRotation,
                visualStartLocalRot,
                bobResetSpeed * Time.deltaTime
            );
        }
        else
        {
            bobTimer += Time.deltaTime;

            float wave = Mathf.Sin(bobTimer * bobFrequency * Mathf.PI * 2f);
            float bobY = wave * bobAmplitude;
            float tiltZ = wave * tiltAmplitude;

            visual.localPosition =
                visualStartLocalPos + Vector3.up * bobY;

            visual.localRotation =
                visualStartLocalRot * Quaternion.Euler(0f, 0f, tiltZ);
        }
    }

    // Engel ile çarpışıldığında knockback tetikler
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
            return;

        if (!collision.gameObject.CompareTag("Obstacle"))
            return;

        Vector3 hitDirection = transform.position - collision.contacts[0].point;
        hitDirection.z = 0f;
        hitDirection.Normalize();

        StartCoroutine(ApplyKnockback(hitDirection));
    }

    // Kısa süreli geri itme kuvveti uygular
    private IEnumerator ApplyKnockback(Vector3 direction)
    {
        isKnockbacked = true;

        rb.velocity = Vector3.zero;
        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockbacked = false;
    }
}
