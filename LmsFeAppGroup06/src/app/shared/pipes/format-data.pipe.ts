import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatData'
})
export class FormatDataPipe implements PipeTransform {

  transform(value: any, ...args: any[]): any {
    if (value.length < args[1])
      return value;
    return value.substr(args[0], args[1]) + '...';
  }

}
