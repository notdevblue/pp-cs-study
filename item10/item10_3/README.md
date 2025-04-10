## 클래스

| Han |
-------
| Object Header (4, 8바이트) |
| Method Table Pointer (4, 8바이트) |
| isNeedCoffee (1비트) |
| age (4바이트) |

---

| Method Table Pointer |
------------------------
| EEClassPointer |
| InterfaceMap |
| ParentTypePointer |
| VTable |
| Non-Virtual |

c++처럼 vtable 만 있는게 아니라 런타임 시 컴파일에도 사용됨