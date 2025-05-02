import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SidebarComponent } from "../../shared-components/sidebar/sidebar.component";

@Component({
  selector: 'app-gallery-page',
  imports: [SidebarComponent],
  templateUrl: './gallery-page.component.html',
  styleUrl: './gallery-page.component.css'
})
export class GalleryPageComponent {
  public shootingId!: number

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.shootingId = params['id']
    });
  }
}
