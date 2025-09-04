using Fusion;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CarCamera : NetworkBehaviour
{
   public Transform target;

   private void Start()
   {
      if (HasStateAuthority)
      {
         FindFirstObjectByType<AutoCam>().Target = target;
      }
   }
}
