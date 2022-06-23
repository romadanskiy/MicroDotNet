package com.example.scanner.presentation.models

class GarbageCategoryModel(val name: String, val id: Long, var selected: Boolean) {

    @JvmName("setSelected1")
    fun setSelected(newSelected: Boolean){
        selected = newSelected
    }

    override fun toString(): String {
        return name
    }
}