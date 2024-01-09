import { Component } from '@angular/core';
import { AddBlogpost } from '../models/add-blog-post.model';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.css']
})
export class AddBlogpostComponent {
  model: AddBlogpost;

  constructor(){
    this.model = {
      title: '',
      shortDescription: '',
      content: '',
      featuredImageUrl: '',
      urlHandle: '',
      author: '',
      publishedDate: new Date(),
      isVisible: true
    }
  }

  onFormSubmit(): void {
    console.log(this.model);
  }
}
