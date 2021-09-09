# SocialNetwork-Kata
El anterior desarrollador que trabajaba en el proyecto fue despedido justo antes de que entrases tu, pero dejo algún código que piensas que puedes reutilizar.

Tú tarea es completar el prototipo, que debe de ser capaz de:

* Anadir miembros
* Conseguir miembros por Id
* Buscar miembros por nombre o información de perfil
* Un miembro puede crear un Post
* Un miembro puede ver sus posts y los de sus amigos. Ordenados por Id de modo ascendente
* Un miembro puede añadir likes aun post
* Un miembro puede añadir otros miembros como amigos.
* Un miembro puede ver una lista de amigos y solicitudes de amistad recibidas.
* Un miembro puede confirmar la amistad con un amigo.
* Un miembro puede conseguir una lista de amigos, amigos de amigos, ... cualquier nivél.


### Detalles de implementación y notas
- Los valores de Member.Id y Post.Id values deben de ser *unicos y secuenciales*. Tienes la clase IdGenerator para que te ayuden con ello pero no se te obliga a usarla. El metodo estático IdGenerator.GetIdPost() o IdGenerator.GetIdMember() devuelve siempre un valor unico y secuencial para posts y miembros.
- Cuidado con las referencias circulares en GetFeed y GetFriends(). No necesitas implementar filtros pero puedes llegara al punto en que lo necesites para evitar esto.
- Observa el código ya que contiene comentarios que te ayudanran con detalles en cada método y propiedad.
