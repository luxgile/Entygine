using Entygine.Cycles;
using OpenTK.Mathematics;

namespace Entygine.Ecs
{
	[SystemGroup(typeof(MainPhases.LatePhaseId))]
	public class TransformConverterSystem : QuerySystem<EntityIterator_Entygine>
	{
		protected override void OnFrame(float dt)
		{
			Iterator.Iterate((ref C_Transform transform, ref C_Position? position, ref C_Rotation? rotation, ref C_UniformScale? scale) =>
			{
				Matrix4 transformValue = Matrix4.Identity;

				Vector3 s = Vector3.One;
				if (scale.HasValue)
					s *= scale.Value.value;

				Quaternion r = Quaternion.Identity;
				if (rotation.HasValue)
					r = rotation.Value.value;

				Vector3 p = Vector3.Zero;
				if (position.HasValue)
					p = (Vector3)position.Value.value;

				transformValue *= Matrix4.CreateScale(s);
				transformValue *= Matrix4.CreateFromQuaternion(r);
				transformValue *= Matrix4.CreateTranslation(p);

				transform.value = transformValue;
			});
		}
	}
}