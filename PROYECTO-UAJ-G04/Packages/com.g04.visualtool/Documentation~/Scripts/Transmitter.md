# Transmitter

Este script debe asociarse a los objetos que emiten el sonido en caso de que el gameobject no contenga la componente AudioSource se la añade de manera automática.

Es el encargado de enviar la informacion del indicador que debe generarse al Indicator Controller para que esta la gestione. La informacion se mantiene desde su construccion hasta su eliminación, esta no se puede cambiar en medio de ejecución.

## Valores editables

* **RawImage** *Image*

 Hace referencia a la imagen del indicador por defecto, esta pensado para imagenes cuadradas cuyo indicador apunta a la derecha usa un material cuyo color se identifica mediante la propiedad _Color, la vibración mediante la propiedad _Vibration y el alfa se contrala mediante la propiedad _Distance

* **float** *Listenable Distance*

Es la distancia hasta la que se enseñará el indicador, no coincide con la de AudioSource

* **Color** *Shader Color* 

El color que se le desea aplicar al Indicador el alpha se ignora porque se calcula internamente con respecto a la distancia

* **float** *Scale Indicator*

Factor por el que se multiplicará el tamaño estandar del indicador.

* **Sprite** *Icon* 

Es un Sprite, si no se pasa ninguna no añadira icono, en caso de que si se añada aparecerá el icono en el indicador.

* **float** *Scale Icon* 

Factor  por el que se multiplicara el sprite tanto en ancho y alto para respetar las dimensiones del sprite

* **float** *Vibration* 

El indicador proporciona una vibracion cuanto mas bajo es el numero en valor absoluto  mas suave y armónica es esta, aunque si es 0 