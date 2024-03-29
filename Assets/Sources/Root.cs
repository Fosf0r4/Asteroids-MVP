using Asteroids.Model;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Root : MonoBehaviour
{
    [SerializeField] private ShipPresenter _shipPresenter;
    [SerializeField] private Camera _camera;
    [SerializeField] private PresentersFactory _factory;
    [SerializeField] private EndGameWindowPresenter _endGameWindow;

    private Ship _shipModel;
    private ShipInputRouter _shipInputRouter;
    private DefaultGun _baseGun;
    private LaserGun _laserGun;
    private LaserGunRollback _laserGunRollback;
    private Score _score;

    public Ship Ship => _shipModel;
    public LaserGun LaserGun => _laserGun;
    public LaserGunRollback LaserGunRollback => _laserGunRollback;
    public ShipPresenter ShipPresenter => _shipPresenter;

    public void GameOver()
    {
        DisableShip();
        OnShipDestroying(); 
    }

    public void OnHit(uint damage)
    {
        _shipPresenter.ShipHealth -= damage;
    }

    public void DisableShip()
    {
        _shipInputRouter.OnDisable();
    }

    private void Awake()    
    {

        _shipModel = new Ship(new Vector2(0.5f, 0.5f), 0);

        _baseGun = new DefaultGun(_shipModel);
        _laserGun = new LaserGun(_shipModel, 10);

        _shipInputRouter = new ShipInputRouter(_shipModel)
            .BindGunToFirstSlot(_baseGun)
            .BindGunToSecondSlot(_laserGun);

        _score = new Score();

        _shipPresenter.Init(_shipModel, _camera);
        _shipPresenter.Init(this);

        _laserGunRollback = new LaserGunRollback(_laserGun, Config.LaserCooldown);

    }

    private void OnEnable()
    {
        _shipInputRouter.OnEnable();

        _baseGun.Shot += OnShot;
        _laserGun.Shot += OnShot;
        _shipModel.Destroying += OnShipDestroying;
    }

    private void OnDisable()
    {
        _shipInputRouter.OnDisable();

        _baseGun.Shot -= OnShot;
        _laserGun.Shot -= OnShot;
        _shipModel.Destroying -= OnShipDestroying;
    }

    private void Update()
    {
        _shipInputRouter.Update();
        _laserGunRollback.Tick(Time.deltaTime);
    }

    private void OnShot(Bullet bullet)
    {
        _factory.CreateBullet(bullet);
    }

    private void OnShipDestroying()
    {
        _endGameWindow.Show(_score.Value, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }
}
