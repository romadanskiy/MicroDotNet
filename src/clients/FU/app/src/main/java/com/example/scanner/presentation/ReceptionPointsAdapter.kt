package com.example.scanner.presentation

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.TextView
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.scanner.domain.models.GetReceptionPoint
import com.example.scanner.R



public class ReceptionPointsAdapter (context: Context, points: List<GetReceptionPoint?>) :
    ArrayAdapter<GetReceptionPoint?>(context, 0, points) {
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
        // Get the data item for this position
        var convertView: View? = convertView
        val point: GetReceptionPoint? = getItem(position)
        // Check if an existing view is being reused, otherwise inflate the view
        if (convertView == null) {
            convertView = LayoutInflater.from(context).inflate(R.layout.reception_point_list_item, parent, false)
        }
        // Lookup view for data population
        val name = convertView?.findViewById(R.id.pointItemName) as TextView
        val description = convertView.findViewById(R.id.pointItemDescription) as TextView
        val address = convertView.findViewById(R.id.pointItemAddress) as TextView
        // Populate the data into the template view using the data object

        if(point!=null){
            name.setText(point?.name)
            description.setText(point?.description)
            address.setText(point?.address?.FullAddress)

            val recyclerView: RecyclerView = convertView.findViewById<RecyclerView>(R.id.garbageTypesRecyclerView)
            recyclerView.layoutManager = GridLayoutManager(this.context, 3)

            recyclerView.adapter =
                GarbageCategoriesRecyclerAdapter(point.garbageTypes)
        }

        // Return the completed view to render on screen
        return convertView!!
    }
}

