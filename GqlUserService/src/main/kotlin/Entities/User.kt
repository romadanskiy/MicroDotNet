package Entities

import org.jetbrains.exposed.dao.IntEntity
import org.jetbrains.exposed.dao.IntEntityClass
import org.jetbrains.exposed.dao.id.EntityID
import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.dao.id.UUIDTable
import org.jetbrains.exposed.sql.Column
import org.jetbrains.exposed.sql.ResultRow
import org.jetbrains.exposed.sql.Table
import java.util.*
import kotlin.math.absoluteValue

//object Users : IntIdTable() {
//    val name = varchar("name", length = 50) // Column<String>
//    val description = varchar("description", length = 50) // Column<String>
//}
//
//class User(id: EntityID<Int>) : IntEntity(id) {
//    companion object : IntEntityClass<User>(Users)
//
//    var name by Users.name
//    var description by Users.description
//}

object Users : UUIDTable() {
    val username: Column<String> = varchar("username", 50)
    val description: Column<String> = varchar("description", 50)
    val rating: Column<Int> = integer("rating")
}

data class User(
    val id: String,
    val username: String,
    val description: String,
    val rating: Int
) {
    companion object {
        fun fromRow(resultRow: ResultRow) = User(
            id = resultRow[Users.id].value.toString(),
            username = resultRow[Users.username],
            description = resultRow[Users.description],
            rating = resultRow[Users.rating]
        )
    }
}
