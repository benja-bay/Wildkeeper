// ==============================
// IEnemyState.cs
// Interfaz de estados para el sistema de máquina de estados del enemigo
// Define el contrato base para todos los estados que puede adoptar un enemigo
// ==============================

namespace Enemies
{
    // Esta interfaz define los tres métodos fundamentales que cualquier estado del enemigo debe implementar:
    // - Enter: se ejecuta una vez al entrar en el estado
    // - Update: se ejecuta en cada frame mientras el estado esté activo
    // - Exit: se ejecuta una vez al salir del estado
    public interface IEnemyState
    {
        // Lógica de inicialización cuando se entra en el estado
        void Enter();

        // Lógica de ejecución constante mientras el estado esté activo
        void Update();

        // Lógica de limpieza o transición al salir del estado
        void Exit();
    }
}
