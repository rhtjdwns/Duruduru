using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : Singleton<SoundManager>
{

    private class SoundData
    {

        private EventInstance _eventInstance;
        public EventInstance EventInstance { get => _eventInstance; }

        private Transform _target;

        private float _min;
        private float _max;

        public SoundData(EventInstance eventInstance, Transform target = null, float min = 0, float max = 0)
        {
            _eventInstance = eventInstance;
            _target = target;
            _min = min;
            _max = max;
        }

        public void UpdateSoundVolume()
        {
            // FMOD 이벤트 인스턴스에 3D 속성을 동적으로 설정
            _eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_target));

            // 소리가 나는 오브젝트와의 거리
            float dist = Vector3.Distance(_target.position, Camera.main.transform.position);

            // 거리에 따른 Volume 값을 0 ~ 1로 계산
            float normalizedVolume = Mathf.Clamp01((_max - dist) / (_max - _min));

            // 정규화 된 값으로 Volume 설정
            _eventInstance.setVolume(normalizedVolume);
        }
    }

    private Dictionary<string, SoundData> _eventInstances = new Dictionary<string, SoundData>();

    private void Update()
    {
        foreach (var data in _eventInstances)
        {
            data.Value.UpdateSoundVolume();
        }
    }

    public void PlaySound(string path, Transform target = null, float min = 0, float max = 0)
    {
        if (!_eventInstances.ContainsKey(path))
        {
            EventInstance newEvent = RuntimeManager.CreateInstance(path);

            _eventInstances[path] = new SoundData(newEvent, target, min, max);

        }

        _eventInstances[path].EventInstance.start();
    }

    public void PlayOneShot(string path, Transform target = null)
    {
        if (target == null)
        {
            RuntimeManager.PlayOneShot(path);
        }
        else
        {
            RuntimeManager.PlayOneShot(path, target.position);
        }
        
    }

    public void StopSound(string path)
    {
        if (_eventInstances.ContainsKey(path))
        {
            _eventInstances[path].EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _eventInstances[path].EventInstance.release(); // release the instance
            _eventInstances.Remove(path);
        }
    }

    public void StopAllSounds()
    {
        foreach (var eventInstance in _eventInstances.Values)
        {
            eventInstance.EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.EventInstance.release();
        }
        _eventInstances.Clear();
    }

    public void SetParameter(string path, string parameterName, float value) // FMOD Studio에 있는 파라미터 제어(파라미터 이름이 같아야 함)
    {
        if (_eventInstances.ContainsKey(path))
        {
            _eventInstances[path].EventInstance.setParameterByName(parameterName, value);
        }
    }

    public void SetMasterVolume(float value = 0.5f)
    {
        Bus _masterBus = RuntimeManager.GetBus("bus:/");
        _masterBus.setVolume(value);
    }

    private void OnDestroy()
    {
        foreach (var eventInstance in _eventInstances.Values)
        {
            eventInstance.EventInstance.release();
        }
        _eventInstances.Clear();
    }
}