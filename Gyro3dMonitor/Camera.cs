using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyro3dMonitor
{
    /// <summary>
    /// カメラクラスの定義
    /// </summary>
    class Camera
    {
        private Matrix4 viewMatrix;  // 現在のビュー行列
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initialViewMatrix">ビュー行列</param>
        public Camera(Matrix4 initialViewMatrix)
        {
            this.viewMatrix = initialViewMatrix;
        }
        /// <summary>
        /// ビュー行列をセット
        /// </summary>
        /// <param name="newViewMatrix"></param>
        public void SetViewMatrix(Matrix4 newViewMatrix)
        {
            this.viewMatrix = newViewMatrix;
        }
        /// <summary>
        /// カメラの位置を取得
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCameraPosition()
        {
            Matrix4 invView = viewMatrix.Inverted();
            return new Vector3(invView.M41, invView.M42, invView.M43);
        }
        /// <summary>
        /// 現在の視線ベクトル（forwardベクトル）を取得
        /// </summary>
        /// <returns></returns>
        public Vector3 GetForwardVector()
        {
            return -new Vector3(viewMatrix.M13, viewMatrix.M23, viewMatrix.M33);
        }
        /// <summary>
        /// 現在の上向きベクトルを取得
        /// </summary>
        /// <returns></returns>
        public Vector3 GetUpVector()
        {
            return new Vector3(viewMatrix.M12, viewMatrix.M22, viewMatrix.M32);
        }
        /// <summary>
        /// 現在の右向きベクトルを取得
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRightVector()
        {
            return new Vector3(viewMatrix.M11, viewMatrix.M21, viewMatrix.M31);
        }
        /// <summary>
        /// カメラを前後左右に移動する
        /// </summary>
        /// <param name="direction">移動方向ベクトル</param>
        /// <param name="distance">移動距離</param>
        public void Move(Vector3 direction, float distance)
        {
            Vector3 position = GetCameraPosition();
            position += direction.Normalized() * distance;

            // 新しいビュー行列を再計算
            Vector3 newTarget = position + GetForwardVector(); // カメラの視線方向を維持
            viewMatrix = Matrix4.LookAt(position, newTarget, GetUpVector());
        }
        /// <summary>
        /// カメラを前進
        /// </summary>
        /// <param name="distance">移動距離</param>
        public void MoveForward(float distance)
        {
            Move(GetForwardVector(), distance);
        }
        /// <summary>
        /// カメラを後退
        /// </summary>
        /// <param name="distance">移動距離</param>
        public void MoveBackward(float distance)
        {
            Move(-GetForwardVector(), distance);
        }
        /// <summary>
        /// カメラを右移動
        /// </summary>
        /// <param name="distance">移動距離</param>
        public void MoveRight(float distance)
        {
            Move(GetRightVector(), distance);
        }
        /// <summary>
        /// カメラを左移動
        /// </summary>
        /// <param name="distance">移動距離</param>
        public void MoveLeft(float distance)
        {
            Move(-GetRightVector(), distance);
        }
        /// <summary>
        /// ズーム倍率を指定してカメラを前後移動（視線方向）
        /// </summary>
        /// <param name="zoomFactor">ズーム係数</param>
        /// <returns></returns>
        public float Zoom(float zoomFactor)
        {
            if (zoomFactor == 1.0f) return 1.0f; // ズーム倍率 1.0 の場合は移動なし

            Vector3 position = GetCameraPosition();
            Vector3 forward = GetForwardVector();
            float zoomDistance = (zoomFactor - 1.0f) * 2.0f; // ズーム倍率を距離に変換

            position += forward * zoomDistance;

            // 新しいビュー行列を再計算
            Vector3 newTarget = position + forward;
            viewMatrix = Matrix4.LookAt(position, newTarget, GetUpVector());

            // 表示設定
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMatrix);
            // ズーム値を返す
            return (float)Math.Sqrt((zoomDistance * zoomDistance) / (position.Length * position.Length));
        }
    }
}
