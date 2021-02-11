using System;
using UnityEngine;

public class FocusParticleManager : Singleton<FocusParticleManager>
{
    protected FocusParticleManager(){}
    [SerializeField] public GameObject particles;
    private Vector3 _focusParticlesPosition;
    private Vector3 _focusParticlesTargetPosition;
    private float _speed = 5.0f;

    private void Start()
    {
        _focusParticlesPosition = particles.transform.position;
        _focusParticlesTargetPosition = _focusParticlesPosition;
    }

    public void UpdateFocusParticles(Vector2 vector2)
    {
        var v3 = new Vector3(vector2.x, vector2.y, 10.0f);
        _focusParticlesTargetPosition = v3;
    }

    private void Update()
    {
        float step = _speed * Time.deltaTime;
        particles.transform.position = Vector3.MoveTowards(particles.transform.position, _focusParticlesTargetPosition, step);
    }
}
