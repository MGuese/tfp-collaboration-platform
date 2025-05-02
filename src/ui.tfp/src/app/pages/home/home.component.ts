import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SidebarComponent } from "../../shared-components/sidebar/sidebar.component";

@Component({
  selector: 'app-home',
  imports: [RouterLink, SidebarComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
