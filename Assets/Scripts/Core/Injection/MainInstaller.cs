using UnityEngine;
using Zenject;

namespace GameOfLife.Core.Injection
{
    [CreateAssetMenu(fileName = "MainInstaller", menuName = "Installers/MainInstaller")]
    public class MainInstaller : ScriptableObjectInstaller<MainInstaller> 
    {
        public override void InstallBindings() { }
    }
}