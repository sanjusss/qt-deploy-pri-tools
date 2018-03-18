copy_src = $$shell_path($$absolute_path($$PWD/{0}))
copy_des = $$shell_path($$absolute_path($$DESTDIR))

package_bin_{1}.src.dirs = $$copy_src
package_bin_{1}.des = $$copy_des
COPIES += package_bin_{1}

unset(copy_src)
unset(copy_des)
